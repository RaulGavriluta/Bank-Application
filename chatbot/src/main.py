import os
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from dotenv import load_dotenv
from chatbot import Chatbot
from pdf_utils import load_pdfs, retrieve_relevant_chunks

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

load_dotenv()
api_key = os.getenv("GEMINI_API_KEY")
if not api_key:
    raise Exception("GEMINI_API_KEY not set in environment variables")

chatbot = Chatbot(api_key, model_name="models/gemini-2.5-flash")

pdf_files = ["raiffeisen-brosura.pdf", "Tarife.pdf", "Termeni-legali.pdf", "Politica_Cookie.pdf"]  
vectorstore, documents = load_pdfs(pdf_files)

class ChatRequest(BaseModel):
    question: str

class ChatResponse(BaseModel):
    answer: str

@app.post("/chat", response_model=ChatResponse)
async def chat_endpoint(request: ChatRequest):
    user_question = request.question.strip()
    if not user_question:
        return {"answer": "Te rog să pui o întrebare."}

    relevant_chunks = retrieve_relevant_chunks(user_question, vectorstore, top_k=5)
    context = "\n\n".join(relevant_chunks)

    prompt = f"""
Esti un asistent virtual prietenos și de ajutor pentru clienți care au întrebări despre servicii bancare.
Răspunde cât mai clar și concis, folosind doar informațiile din context. Nu face ghiciri, nu inventa.
Context:
{context}

Întrebare:
{user_question}

Răspuns:
"""
    answer = chatbot.send_message(prompt)
    return {"answer": answer}
