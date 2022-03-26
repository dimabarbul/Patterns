# Patterns

## Description

The program displays grid of color cells which changes over time according to some rules. Example of such rules is [life game](https://en.m.wikipedia.org/wiki/Conway%27s_Game_of_Life).
Application has shared core and several UI's.
Example of console version displayed using ASCII printer:
```
.x..x.
..xx..
.x.x.x
xx.x..
.x....
xxx.xx
```

## How to Build

To build frontend for Patterns.Web:
```bash
npm i
npm run build
```

To build backend for Patterns.Web or Patterns.Console:
```bash
dotnet build
```

## Diagrams

### Core

```mermaid
classDiagram

class Cell {
  +Color color
}

class IAlgorithm {
  <<interface>>
  GetNext() Cell[][]
}

class Life {
  -Cell[][] cells
  +Constructor(int width, int height)
  +GetNext() Cell[][]
}

class Flame {
  -Color[] Palette$
  -int[][] paletteIndexes
  +Constructor(int width, int height)
  +GetNext() Cell[][]
}

IAlgorithm <|.. Life
IAlgorithm <|.. Flame
```

```mermaid
classDiagram

class AlgorithmType {
  <<enumeration>>
  Life
  Colors
  Flame
  Stars
}

class AlgorithmFactory {
  +Create(AlgorithmType type, IDictionary~string, string~ args) IAlgorithm
}
```

### Console Application

```mermaid
classDiagram

class IPrinter {
  <<interface>>
  Print(Cell[][] cells)
}

class ColorPrinter {
  +Print(Cell[][] cells)
}

class AsciiPrinter {
  +Print(Cell[][] cells)
}

IPrinter <|.. ColorPrinter
IPrinter <|.. AsciiPrinter
```

```mermaid
sequenceDiagram

participant p as Program
participant af as Algorithm Factory
participant t as Timer
participant a as Algorithm
participant pr as Printer

p ->> p: parse CLI arguments
p ->> af: create(type, args)
af -->> p: algorithm
p ->> p: create timer and printer
p ->> t: start()
loop onTimer
  t ->> p: onNextFrame()
  p ->> a: getNext()
  a -->> p: cells
  p ->> pr: print(cells)
end
```

### Web Application with SignalR

```mermaid
sequenceDiagram

participant ui as Browser
participant hub as SignalR Hub
participant w as Worker
participant af as Algorithm Factory
participant t as Timer
participant a as Algorithm

ui ->> hub: connect()
ui ->> hub: start(type, args)
hub ->> w: start(connectionId, <br>type, args)
w ->> af: create(type, args)
af -->> w: algorithm
w ->> w: create timer
w ->> t: start()
loop onTimer
  t ->> w: onNextFrame()
  w ->> a: getNext()
  a -->> w: cells
  w ->> hub: sendCells(connectionId, cells)
  hub ->> ui: cells
  ui ->> ui: display cells
end

ui ->> hub: stop() or disconnect()
hub ->> w: stop(connectionId)
w ->> t: stop()
w ->> w: delete algorithm and timer
```
