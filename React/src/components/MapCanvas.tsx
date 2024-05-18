import { useRef, useEffect, useState } from "react";
import defaultImage from "../assets/Default.png";
import corridorImage from "../assets/Corridor.png";
import corridor90Image from "../assets/Corridor90.png";
import { socket } from "../socket";

class Door {
  location: string;
  isOneWay: boolean;
  isLocked: boolean;
  constructor(location: string, isOneWay: boolean, isLocked: boolean) {
    this.location = location;
    this.isOneWay = isOneWay;
    this.isLocked = isLocked;
  }
}

class Module {
  typeOfModule: string;
  xCoordinate: number;
  yCoordinate: number;
  rotation: number;
  doors: Door[];
  constructor(
    typeOfModule: string,
    xCoordinate: number,
    yCoordinate: number,
    rotation: number,
    doors: Door[]
  ) {
    this.typeOfModule = typeOfModule;
    this.xCoordinate = xCoordinate;
    this.yCoordinate = yCoordinate;
    this.rotation = rotation;
    this.doors = doors;
  }
}

export default function MapCanvas() {
  socket.on("map", (data) => {
    getModules(data);
  });
  const [modules, setModules] = useState<Module[]>([]);
  function getModules(dataToProcess: string) {
    // separating string/array into modules
    const dirtyModules: string[] = dataToProcess.split("|");

    // remove " " from the end of the array
    dirtyModules.pop();
    console.log(dirtyModules);

    // returning array without the element starting with P - remove the player
    const cleanModules = dirtyModules.filter((item) => {
      return item[0] != "P";
    });

    // creating modules array that will be later used as components
    const myModules = cleanModules.map((module) => {
      const splits = module.split("#");
      const textDoors = splits[4].split("-");
      textDoors.pop();
      const doors = textDoors.map((door) => {
        return new Door(door[5], door[8] != null, false);
      });
      // ^ check if the 8th element exists - if yes - return true (one way door), if doesn't (two way door) - return false

      // new Module from cleanModules.map()
      return new Module(
        splits[0],
        parseInt(splits[1]),
        parseInt(splits[2]),
        parseInt(splits[3]),
        doors
      );
    });

    setModules(myModules);
  }

  const canvasRef = useRef(null);

  const grid_X = 8;
  const grid_Y = 8;
  const tileSize = 8;

  useEffect(() => {
    console.log("canvas effect");
    if (canvasRef.current) {
      const canvas: HTMLCanvasElement = canvasRef.current;
      canvas.width = grid_X * tileSize;
      canvas.height = grid_Y * tileSize;
      const context = canvas.getContext("2d");
      if (context) {
        context.fillStyle = "red";
        context.fillRect(0, 0, canvas.width, canvas.height);
      }
      for (const module of modules) {
        const image = new Image();
        if (module.typeOfModule === "Corridor") {
          if (module.rotation !== 0 || 180) {
            image.src = corridor90Image;
          } else {
            image.src = corridorImage;
          }
        } else {
          image.src = defaultImage;
        }
        image.onload = () =>
          context?.drawImage(
            image,
            module.xCoordinate * tileSize,
            module.yCoordinate * tileSize
          );
      }
    }
  }, [modules]);

  return (
    <>
      <canvas ref={canvasRef} className="canvas-map"></canvas>
    </>
  );
}
