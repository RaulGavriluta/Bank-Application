import os
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from dotenv import load_dotenv
from chatbot import Chatbot
from pdf_utils import extract_text_from_pdf, extract_metadata, chunk_by_articles, embed_chunks, retrieve_relevant_chunks

app = FastAPI()

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Încarcă variabilele de mediu
load_dotenv()
api_key = os.getenv("GEMINI_API_KEY")
if not api_key:
    raise Exception("GEMINI_API_KEY not set in environment variables")

# Initializează chatbot-ul
chatbot = Chatbot(api_key, model_name="models/gemini-2.5-flash")

# Încarcă și procesează PDF-ul
pdf_path = "fia.pdf"
if not os.path.exists(pdf_path):
    raise FileNotFoundError(f"PDF file '{pdf_path}' not found.")

pdf_content = extract_text_from_pdf(pdf_path)
metadata = extract_metadata(pdf_content)
chunks = chunk_by_articles(pdf_content)
chunk_embeddings = embed_chunks(chunks)

# Request / Response models
class ChatRequest(BaseModel):
    question: str

class ChatResponse(BaseModel):
    answer: str

# Endpoint
@app.post("/chat", response_model=ChatResponse)
async def chat_endpoint(request: ChatRequest):
    user_question = request.question.strip()

    if "title" in user_question.lower() and "document" in user_question.lower():
        return {"answer": f"Document title → {metadata['title']}"}
    if "publish" in user_question.lower() or "date" in user_question.lower():
        return {"answer": f"Publish date → {metadata['publish_date']}"}

    relevant_chunks = retrieve_relevant_chunks(user_question, chunks, chunk_embeddings, top_k=5)
    context = "\n\n".join(relevant_chunks)

    prompt = f"""
You are an assistant that answers questions strictly using the provided context.
Context:
{context}

Question:
{user_question}

Answer:
"""
    answer = chatbot.send_message(prompt)
    return {"answer": answer}
