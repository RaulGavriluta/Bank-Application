import os
import PyPDF2
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.embeddings import HuggingFaceEmbeddings
from langchain.vectorstores import Chroma
from langchain.schema import Document

# -----------------------
# Extrage textul din PDF
# -----------------------
def extract_text_from_pdf(pdf_path: str) -> str:
    text = ""
    with open(pdf_path, "rb") as f:
        reader = PyPDF2.PdfReader(f)
        for page in reader.pages:
            text += page.extract_text() or ""
    return text

# -----------------------
# Încarcă mai multe PDF-uri și creează vectorstore
# -----------------------
def load_pdfs(pdf_files: list[str], persist_directory="chroma_db"):
    documents = []

    for pdf_path in pdf_files:
        if not os.path.exists(pdf_path):
            print(f"[WARN] Fișierul {pdf_path} nu există, ignorat.")
            continue

        text = extract_text_from_pdf(pdf_path)
        if not text.strip():
            print(f"[WARN] Fișierul {pdf_path} nu conține text.")
            continue

        splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=100)
        chunks = splitter.split_text(text)

        for chunk in chunks:
            documents.append(Document(page_content=chunk, metadata={"source": pdf_path}))

    if not documents:
        raise ValueError("❌ Niciun PDF valid nu a fost încărcat.")

    embeddings = HuggingFaceEmbeddings(model_name="sentence-transformers/all-MiniLM-L6-v2")

    vectorstore = Chroma.from_documents(
        documents=documents,
        embedding=embeddings,
        persist_directory=persist_directory
    )

    return vectorstore, documents

# -----------------------
# RAG retrieval
# -----------------------
def retrieve_relevant_chunks(query: str, vectorstore, top_k=5):
    results = vectorstore.similarity_search(query, k=top_k)
    return [doc.page_content for doc in results]
