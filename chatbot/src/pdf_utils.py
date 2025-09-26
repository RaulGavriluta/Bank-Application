import PyPDF2
from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
import numpy as np

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


def chunk_text(text: str, chunk_size: int = 500) -> list[str]:
    
    chunks = []
    text = text.strip()
    while text:
        chunk = text[:chunk_size]
        chunks.append(chunk)
        text = text[chunk_size:]
    return chunks

# -----------------------
# Embedding
# -----------------------
embedding_model = SentenceTransformer('all-MiniLM-L6-v2')

def embed_chunks(chunks: list[str]) -> np.ndarray:
    if not chunks:
        return np.array([])
    return embedding_model.encode(chunks)

# -----------------------
# RAG retrieval
# -----------------------
def retrieve_relevant_chunks(query: str, chunks: list[str], chunk_embeddings: np.ndarray, top_k=5) -> list[str]:
    if not chunks or chunk_embeddings.size == 0:
        return ["Document has no content."]

    query_embedding = embedding_model.encode([query])
    similarities = cosine_similarity(query_embedding, chunk_embeddings)[0]

    top_indices = similarities.argsort()[::-1][:top_k]
    return [chunks[i] for i in top_indices]
