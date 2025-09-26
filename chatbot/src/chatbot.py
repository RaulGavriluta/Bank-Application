import google.generativeai as genai

class Chatbot:
    def __init__(self, api_key, model_name='models/gemini-2.5-pro'):
        genai.configure(api_key=api_key)
        self.model = genai.GenerativeModel(model_name)

    def send_message(self, message):
        try:
            response = self.model.generate_content(message)
            return response.text
        except Exception as e:
            return f"Error: {e}"

def list_models(api_key):
    genai.configure(api_key=api_key)
    print("Available models:")
    for m in genai.list_models():
        print(m.name)