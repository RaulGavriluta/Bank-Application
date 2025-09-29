import os
import re
import PyPDF2
import numpy as np
from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.preprocessing import normalize

# -----------------------
# Extrage textul din PDF
# -----------------------
def extract_text_from_pdf(pdf_path: str) -> str:
    text = ""
    with open(pdf_path, "rb") as f:
        reader = PyPDF2.PdfReader(f)
        for page in reader.pages:
            text += page.extract_text() or ""
    return text.strip()


# -----------------------
# Chunking inteligent
# -----------------------
def chunk_text(text: str, chunk_size: int = 500, overlap: int = 100) -> list[str]:
    sentences = re.split(r'(?<=[.!?]) +', text)  # spargem în propoziții
    chunks, current_chunk = [], ""

    for sentence in sentences:
        if len(current_chunk) + len(sentence) <= chunk_size:
            current_chunk += " " + sentence
        else:
            chunks.append(current_chunk.strip())
            current_chunk = sentence

    if current_chunk:
        chunks.append(current_chunk.strip())

    # suprapunere între fragmente
    final_chunks = []
    for i in range(len(chunks)):
        start = max(0, i - 1)
        merged = " ".join(chunks[start:i+1])
        final_chunks.append(merged.strip())

    return final_chunks


# -----------------------
# Embeddings
# -----------------------
embedding_model = SentenceTransformer("all-MiniLM-L6-v2")

def embed_chunks(chunks: list[str]) -> np.ndarray:
    if not chunks:
        return np.array([])
    return embedding_model.encode(chunks, convert_to_numpy=True)


# -----------------------
# Retrieval
# -----------------------
def retrieve_relevant_chunks(query: str, chunks: list[str], chunk_embeddings: np.ndarray, top_k=5) -> list[str]:
    if not chunks or chunk_embeddings.size == 0:
        return ["Documentul nu conține text."]

    # Embedding pentru întrebare
    query_embedding = embedding_model.encode([query], convert_to_numpy=True)
    query_embedding = normalize(query_embedding.reshape(1, -1))

    # Similaritate cosinus
    similarities = cosine_similarity(query_embedding, normalize(chunk_embeddings))[0]
    top_indices = similarities.argsort()[::-1][:top_k]

    # Eliminăm duplicatele
    seen, results = set(), []
    for idx in top_indices:
        if chunks[idx] not in seen:
            results.append(chunks[idx])
            seen.add(chunks[idx])

    return results


# -----------------------
# Preprocesare pentru mai multe PDF-uri
# -----------------------
def load_multiple_pdfs(pdf_paths: list[str]):
    """
    Încarcă mai multe PDF-uri, le procesează și returnează
    toate fragmentele + embeddings într-o singură colecție.
    """
    all_chunks, all_embeddings, sources = [], [], []

    for path in pdf_paths:
        if not os.path.exists(path):
            print(f"⚠️ Fișierul {path} nu există, îl sar.")
            continue

        text = extract_text_from_pdf(path)
        chunks = chunk_text(text)
        embeddings = embed_chunks(chunks)

        all_chunks.extend(chunks)
        sources.extend([os.path.basename(path)] * len(chunks))

        if embeddings.size > 0:
            if len(all_embeddings) == 0:
                all_embeddings = embeddings
            else:
                all_embeddings = np.vstack((all_embeddings, embeddings))

    return all_chunks, all_embeddings, sources
