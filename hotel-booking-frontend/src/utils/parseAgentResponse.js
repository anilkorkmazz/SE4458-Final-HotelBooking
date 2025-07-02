export const parseAgentResponse = (text) => {
    const blocks = text.trim().split("\n\n").filter(Boolean);
    return blocks.map((block) => {
      const lines = block.split("\n").map((l) => l.trim());
  
      return {
        hotel: lines[0]?.replace("🏨 Hotel: ", ""),
        location: lines[1]?.replace("📍 Location: ", ""),
        room: lines[2]?.replace("🛏️ Room: ", ""),
        price: lines[3]?.replace("💰 Price per night: ", ""),
        availability: lines[4]?.replace("📅 Available: ", ""),
      };
    });
  };