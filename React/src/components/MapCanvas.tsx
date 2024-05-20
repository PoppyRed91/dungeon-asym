import { useRef, useEffect, useState } from "react";
import defaultImage from "../assets/Default.png";
import corridorImage from "../assets/Corridor.png";
import corridor90Image from "../assets/Corridor90.png";
import doorN from "../assets/DoorN.png";
import doorE from "../assets/DoorE.png";
import doorS from "../assets/DoorS.png";
import doorW from "../assets/DoorW.png";
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
      context?.setTransform(1, 0, 0, -1, 0, canvas.height);
      if (context) {
        context.fillStyle = "red";
        context.fillRect(0, 0, canvas.width, canvas.height);
      }
      for (const module of modules) {
        const image = new Image();
        if (module.typeOfModule === "Corridor") {
          if (module.rotation === 0 || module.rotation === 180){
            image.src = corridorImage;
          } else {
            image.src = corridor90Image;
          }
        } 
        else {
          image.src = defaultImage;
        }
        image.onload = () =>
          context?.drawImage(
            image,
            module.xCoordinate,
            module.yCoordinate
          );
        for (const door of module.doors) {
          const image = new Image();
          switch (door.location) {
            case "N":
              image.src = doorN;
              image.onload =  () => {
                context?.drawImage(image, module.xCoordinate, module.yCoordinate);
              }
              break;
            case "E":
              image.src = doorE;
              image.onload =  () => {
                context?.drawImage(image, module.xCoordinate, module.yCoordinate);
              }
              break;
            case "S":
              image.src = doorS;
              image.onload =  () => {
                context?.drawImage(image, module.xCoordinate, module.yCoordinate);
              }
              break;
            case "W":
              image.src = doorW;
              image.onload =  () => {
                context?.drawImage(image, module.xCoordinate, module.yCoordinate);
              }
              break;
              case "N_OW":
                image.src = doorN;
                image.onload =  () => {
                  context?.drawImage(image, module.xCoordinate, module.yCoordinate);
                }
                break;
              case "E_OW":
                image.src = doorE;
                image.onload =  () => {
                  context?.drawImage(image, module.xCoordinate, module.yCoordinate);
                }
                break;
              case "S_OW":
                image.src = doorS;
                image.onload =  () => {
                  context?.drawImage(image, module.xCoordinate, module.yCoordinate);
                }
                break;
              case "W_OW":
                image.src = doorW;
                image.onload =  () => {
                  context?.drawImage(image, module.xCoordinate, module.yCoordinate);
                }
                break;
          }
        }
      }
    }
  }, [modules]);

  return (
    <>
      <canvas ref={canvasRef} className="canvas-map"></canvas>
    </>
  );
}
