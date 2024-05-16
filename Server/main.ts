import { serve } from "https://deno.land/std@0.150.0/http/server.ts";
import { Server } from "https://deno.land/x/socket_io@0.1.1/mod.ts";

const server = new Server({
  cors: {
    origin: "http://192.168.50.3:3000",
  },
});

server.on("connection", (client) => {
  let nickname: string;

  client.on("userdata", (name) => {
    nickname = name;
    log(`${nickname} connected!`);
    server.emit("msg", nickname + " just joined!");
    server.emit("msg", "Welcome " + nickname + "!!");
  });

  client.on("msg", (data) => {
    log(nickname + ": " + data);
  });

  client.on("map", (data) => {
    RelayMessage("map", data);
  });

  client.on("player", (data) => {
    RelayMessage("player", data);
  });

  client.on("compass", (data) => {
    RelayMessage("compass", data);
  });

  client.on("disconnect", (reason) => {
    log(`${nickname} disconnected due to ${reason}`);
    server.emit("msg", `${nickname} disconnected due to ${reason}`);
  });

  function RelayMessage(message: string, data: string) {
    server.emit(message, data);
    log("Relaying message: " + message);
  }
});

await serve(server.handler(), {
  hostname: "192.168.50.3",
  port: 3000,
});

function log(text: string) {
  const timestamp = new Date();
  console.log(
    "\x1b[0m" + "[" +
      "\x1b[32m",
    timestamp + "\x1b[0m" + "]",
    " ",
    text,
  );
}
