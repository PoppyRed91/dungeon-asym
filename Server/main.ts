import { serve } from "https://deno.land/std@0.150.0/http/server.ts";
import { Server } from "https://deno.land/x/socket_io@0.1.1/mod.ts";

const io = new Server();

io.on("connection", (socket) => {
  let nickname: string;
  socket.on("userdata", (name) => {
    nickname = name;
    log(`${nickname} connected!`);
  });

  socket.on("msg", (data) => {
    log(nickname + ": " + data);
  });

  socket.on("disconnect", (reason) => {
    log(`${nickname} disconnected due to ${reason}`);
  });
});

await serve(io.handler(), {
  hostname: "0.0.0.0",
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
