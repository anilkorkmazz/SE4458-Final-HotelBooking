const BASE_URL = "http://localhost:5009"; // Ocelot Gateway

export const sendAgentMessage = async (message) => {
  try {
    const response = await fetch(`${BASE_URL}/api/v1/agent/message`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(message), 
    });

    if (!response.ok) {
      throw new Error("Agent request failed");
    }

    const data = await response.json(); 
    return data.message;
  } catch (error) {
    console.error("Agent Error:", error);
    return "❌ Something went wrong.";
  }
};