import os
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from dotenv import load_dotenv
from chatbot import Chatbot
from pdf_utils import extract_text_from_pdf, chunk_text, embed_chunks, retrieve_relevant_chunks

# -----------------------
# Setup FastAPI
# -----------------------
app = FastAPI()

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],  # URL frontend React
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# -----------------------
# Load environment
# -----------------------
load_dotenv()
api_key = os.getenv("GEMINI_API_KEY")
if not api_key:
    raise Exception("GEMINI_API_KEY not set in environment variables")

# -----------------------
# Initialize chatbot
# -----------------------
chatbot = Chatbot(api_key, model_name="models/gemini-2.5-flash")

# -----------------------
# Load PDF and prepare embeddings
# -----------------------
pdf_path = "raiffeisen-brosura.pdf"
if not os.path.exists(pdf_path):
    raise FileNotFoundError(f"PDF file '{pdf_path}' not found.")

pdf_content = extract_text_from_pdf(pdf_path)
chunks = chunk_text(pdf_content, chunk_size=500)
chunk_embeddings = embed_chunks(chunks)

# -----------------------
# Request / Response models
# -----------------------
class ChatRequest(BaseModel):
    question: str

class ChatResponse(BaseModel):
    answer: str

# -----------------------
# Chat endpoint
# -----------------------
@app.post("/chat", response_model=ChatResponse)
async def chat_endpoint(request: ChatRequest):
    user_question = request.question.strip()
    if not user_question:
        return {"answer": "Please provide a question."}

    relevant_chunks = retrieve_relevant_chunks(user_question, chunks, chunk_embeddings, top_k=5)
    context = "\n\n".join(relevant_chunks)

    # Prompt simplu pentru RAG
    prompt = f"""
Esti un asistent virtual prietenos si de ajutor pentru un client care are intrebari despre serviciile bancare. Raspunde la intrebari cat mai clar si prietenos. Afiseaza raspunsurile intr-un mod concis si usor de inteles.Nu oferi raspunsuri prea lungi.Nu saluta de mai multe ori pe parcursul conversatiei, doar o data, la inceput. Raspunde la intrebari folosind urmatorul context:
Context:
{context}

Question:
{user_question}

Answer:
"""
    answer = chatbot.send_message(prompt)
    return {"answer": answer}
