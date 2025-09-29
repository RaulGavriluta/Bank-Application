import os
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from dotenv import load_dotenv
from chatbot import Chatbot
from pdf_utils import load_multiple_pdfs, retrieve_relevant_chunks

# -----------------------
# Setup FastAPI
# -----------------------
app = FastAPI()

# CORS middleware (pentru frontend React)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],  # URL-ul aplicației tale frontend
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
    raise Exception("❌ GEMINI_API_KEY not set in environment variables")

# -----------------------
# Initialize chatbot
# -----------------------
chatbot = Chatbot(api_key, model_name="models/gemini-2.5-flash")

# -----------------------
# Load multiple PDFs
# -----------------------
pdf_paths = [
    "raiffeisen-brosura.pdf",
    "tarife.pdf",
]

chunks, chunk_embeddings, sources = load_multiple_pdfs(pdf_paths)

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
        return {"answer": "Te rog să scrii o întrebare legată de produsele sau serviciile bancare."}

    relevant_chunks = retrieve_relevant_chunks(user_question, chunks, chunk_embeddings, top_k=5)
    context = "\n\n".join(relevant_chunks)

    # Prompt îmbunătățit în română
    prompt = f"""
Ești un asistent virtual prietenos al unei bănci. 
Răspunzi la întrebările clienților folosind strict informațiile din documentele oficiale puse la dispoziție (broșuri, tarife, contracte).  

Instrucțiuni:  
- Fii clar și prietenos.  
- Explică pe scurt și pe înțelesul clientului.  
- Nu inventa informații, răspunde doar dacă există în context.  
- Dacă nu găsești răspunsul în documente, spune politicos că informația nu este disponibilă.  
- Evită răspunsurile prea lungi.  

Context:
{context}

Întrebare client:
{user_question}

Răspuns:
"""

    answer = chatbot.send_message(prompt)
    return {"answer": answer}
