import PyPDF2
from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
import numpy as np
import re

# Citește textul din PDF
def extract_text_from_pdf(pdf_path):
    text = ""
    with open(pdf_path, "rb") as f:
        reader = PyPDF2.PdfReader(f)
        for page in reader.pages:
            text += page.extract_text() or ""
    return text

# Extrage titlu și data publicării
def extract_metadata(text):
    intro_text = text[:1000]
    title_match = re.search(r"(FIA.*?Regulations.*?)\n", intro_text, flags=re.IGNORECASE)
    date_match = re.search(r"(?:published|issue[d]? on|date:)\s*(.*)", intro_text, flags=re.IGNORECASE)
    title = title_match.group(1).strip() if title_match else "Not found"
    publish_date = date_match.group(1).strip() if date_match else "Not found"
    return {"title": title, "publish_date": publish_date}

# Împarte PDF-ul în articole (chunk-uri)
def chunk_by_articles(text):
    matches = re.split(r"(Article\s+\d+\s*(?:–|-|:)?\s*.*)", text, flags=re.IGNORECASE)
    chunks = []
    for i in range(1, len(matches), 2):
        heading = matches[i].strip()
        body = matches[i+1].strip() if i+1 < len(matches) else ""
        chunks.append(f"{heading}\n{body}")
    return chunks

# Creează embeddings pentru fiecare chunk
embedding_model = SentenceTransformer('all-MiniLM-L6-v2')
def embed_chunks(chunks):
    return embedding_model.encode(chunks)

# Găsește chunk-urile relevante
def retrieve_relevant_chunks(query, chunks, chunk_embeddings, top_k=5, always_include_first=2):
    query_embedding = embedding_model.encode([query])
    similarities = cosine_similarity(query_embedding, chunk_embeddings)[0]

    keyword_boost = np.array([
        0.3 if re.search(r"Article\s+\d+\s*(?:–|-|:)?\s*\w*", chunk, flags=re.IGNORECASE) else 0.0
        for chunk in chunks
    ])

    scores = similarities + keyword_boost
    top_indices = np.argsort(scores)[::-1][:top_k]

    extra_indices = list(range(min(always_include_first, len(chunks))))
    final_indices = list(dict.fromkeys(extra_indices + list(top_indices)))
    return [chunks[i] for i in final_indices]
