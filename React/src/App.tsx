import { useEffect } from "react";
import { io } from "socket.io-client";
import "./App.css";

export default function App() {
  const socket = io("ws://192.168.50.3:3000", {
    transports: ["websocket"],
    autoConnect: false,
  });

  function connect() {
    socket.connect();
  }

  function disconnect() {
    socket.disconnect();
  }

  useEffect(() => {
    console.log("effect");
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

    return () => {
      socket.disconnect();
    };
  }, [socket]);

  return (
    <section>
      <p>Navigate through dungeon</p>
      <button onClick={() => connect()}>
        <p>Join</p>
      </button>
      <button onClick={() => disconnect()}>
        <p>Disconnect</p>
      </button>
    </section>
  );
}
