function init() {
    const canvas = document.getElementById('canvas');
    const context = canvas.getContext('2d')
        || (() => {
            throw new Error('Canvas context is not available.');
        })();

    function displaySingleCell(imageData, cell, column, row) {
        const baseColorIndex = row * 4 * imageData.width + column * 4;
        imageData.data[baseColorIndex] = cell[0];
        imageData.data[baseColorIndex + 1] = cell[1];
        imageData.data[baseColorIndex + 2] = cell[2];
        imageData.data[baseColorIndex + 3] = 255;
    }

    function displayCell(imageData, cell, column, row, size) {
        for (let i = 0; i < size; i++) {
            for (let j = 0; j < size; j++) {
                displaySingleCell(imageData, cell, column * size + i, row * size + j);
            }
        }
    }

    function render(cells, size) {
        const start = Date.now();
        const imageData = context.createImageData(canvas.width, canvas.height);

        for (let i = 0; i < cells.length; i++) {
            for (let j = 0; j < cells[i].length; j++) {
                displayCell(imageData, cells[i][j], i, j, parseInt(size));
            }
        }
        context.putImageData(imageData, 0, 0);
        const stop = Date.now();
        console.log(`render in JS ${stop - start} ms`);
    }

    function clear() {
        context.clearRect(0, 0, canvas.width, canvas.height);
    }

    window.render = render;
    window.clear = clear;
}
