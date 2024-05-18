import { io } from "socket.io-client";

export const socket = io("ws://192.168.50.3:3000", {
  transports: ["websocket"],
  autoConnect: false,
});

export function connect() {
  socket.connect();
}

export function disconnect() {
  socket.disconnect();
}

socket.on("connect", () => {
  console.log("Connected to server big success");
  socket.emit("userdata", "zanovijetalo");
});

socket.on("msg", (data) => {
  console.log("Received message:", data);
});

socket.on("disconnect", () => {
  console.log("Disconnected from server.");
});

window.onbeforeunload = () => {
  socket.disconnect();
};
