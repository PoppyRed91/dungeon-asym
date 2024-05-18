import { connect, disconnect } from "../socket";
import { Link } from "react-router-dom";

export default function HomePage() {
  return (
    <section>
      <p>Navigate through dungeon</p>
      <Link to="game">
        <button onClick={() => connect()}>Join</button>
      </Link>
      <button onClick={() => disconnect()}>Disconnect</button>
    </section>
  );
}
