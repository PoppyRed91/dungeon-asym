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

export default function MapGrid() {
  const dataFromServerForGrid: string =
    "Start#14#10#180#Door_N-|Default#14#9#180#Door_E-|Corridor#12#8#0#Door_W-|Default#12#9#180#Door_E-|Default#10#8#0#Door_S-Door_N-|Corridor#11#7#270#Door_W-|Default#11#6#270#Door_W-|Default#11#6#180#Door_E-Door_W-|Default#10#6#180#Door_N-|Exit#9#4#0#|Corridor#11#5#0#Door_E-|Default#12#6#90#Door_E-|Default#12#5#90#Door_S-|Corridor#12#5#180#Door_E-|Default#10#4#0#Door_N_OW-|Default#11#10#180#Door_E-|Corridor#9#9#0#Door_W-|Default#9#9#270#Door_W-|Default#8#8#0#Door_E-|Default#9#9#90#Door_E-Door_N_OW-|Corridor#9#8#90#Door_E-|Default#9#6#0#Door_W-|Default#8#6#0#Door_S-|Default#8#6#90#Door_N_OW-|Player#14#10#180#|";

  function getModules(dataToProcess: string): Module[] {
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
    const modules = cleanModules.map((module) => {
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

    return modules;
  }

  const myModules = getModules(dataFromServerForGrid);
  console.log(myModules);

  return <></>;
}
