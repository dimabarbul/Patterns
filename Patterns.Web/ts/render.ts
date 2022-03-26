import { get, State } from "./controls";

const canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
const context: CanvasRenderingContext2D = canvas.getContext('2d')
    || (() => { throw new Error('Canvas context is not available.'); })();

function displaySingleCell(imageData: ImageData, cell: number[], column: number, row: number): void {
    const baseColorIndex = row * 4 * imageData.width + column * 4;
    imageData.data[baseColorIndex] = cell[0];
    imageData.data[baseColorIndex + 1] = cell[1];
    imageData.data[baseColorIndex + 2] = cell[2];
    imageData.data[baseColorIndex + 3] = 255;
}

function displayCell(imageData: ImageData, cell: number[], column: number, row: number, size: number): void {
    for (let i = 0; i < size; i++) {
        for(let j = 0; j < size; j++) {
            displaySingleCell(imageData, cell, column * size + i, row * size + j);
        }
    }
}

export function render(cells: number[][][]): void {
    const state: State = get();
    const imageData: ImageData = context.createImageData(canvas.width, canvas.height);

    for (let i = 0; i < cells.length; i++) {
        for (let j = 0; j < cells[i].length; j++) {
            displayCell(imageData, cells[i][j], i, j, parseInt(state.size));
        }
    }
    context.putImageData(imageData, 0, 0);
}

export function clear(): void {
    context.clearRect(0, 0, canvas.width, canvas.height);
}
