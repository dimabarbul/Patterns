import { get, State } from './controls';

const canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
const context: CanvasRenderingContext2D = canvas.getContext('2d')
    || (() => { throw new Error('Canvas context is not available.'); })();

export function render(width: number, height: number, cells: number[]): void {
    const state: State = get();
    const size: number = parseInt(state.size);

    if (cells.length !== width * height * 3) {
        throw new Error('Invalid number of cells.');
    }
    if (canvas.width !== width * size) {
        throw new Error('Wrong canvas width.');
    }
    if (canvas.height !== height * size) {
        throw new Error('Wrong canvas height.');
    }

    const imageData: ImageData = context.createImageData(canvas.width, canvas.height);

    let imageDataIndex: number = 0;
    let rowStartIndex: number = 0;
    for (let i = 0; i < height; i++, rowStartIndex += width * 3) {
        for(let j = 0; j < size; j++) {
            for(let k = 0; k < width; k++) {
                for(let l = 0; l < size; l++) {
                    let cellStartIndex: number = rowStartIndex + 3 * k;
                    imageData.data[imageDataIndex] = cells[cellStartIndex];
                    imageData.data[imageDataIndex + 1] = cells[cellStartIndex + 1];
                    imageData.data[imageDataIndex + 2] = cells[cellStartIndex + 2];
                    imageData.data[imageDataIndex + 3] = 255;

                    imageDataIndex += 4;
                }
            }
        }
    }
    context.putImageData(imageData, 0, 0);
}

export function clear(): void {
    context.clearRect(0, 0, canvas.width, canvas.height);
}
