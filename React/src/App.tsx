import "./App.css";
import GamePage from "./pages/GamePage";
import { Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage></HomePage>}></Route>
      <Route path="game" element={<GamePage></GamePage>}></Route>
    </Routes>
  );
}
