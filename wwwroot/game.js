let canvas = document.getElementById("gameCanvas");
let ctx = canvas.getContext("2d");
ctx.imageSmoothingEnabled = false;

ctx.font = '15px Arial';
ctx.fillStyle = 'purple'; // Set text color
ctx.textAlign = 'center'; // Align text
ctx.textBaseline = 'middle'; // Align vertically

let uiCanvas = document.getElementById("UiCanvas");
let uiCtx = uiCanvas.getContext("2d");
uiCtx.imageSmoothingEnabled = false;

const offscreenCanvas = document.createElement('canvas');
offscreenCanvas.width = canvas.width;
offscreenCanvas.height = canvas.height;
const offscreenContext = offscreenCanvas.getContext('2d');
offscreenContext.imageSmoothingEnabled = false;


let keyStates = {};

function clearCanvas() {
    return new Promise((resolve) => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);  // Clear canvas
        resolve();
    });
    

}

// Store all preloaded images in an object
var imageCache = {};

async function preloadImage(imagePath) {
    return new Promise((resolve, reject) => {
        let img = new Image();
        img.src = imagePath;

        img.onload = async () => {
            // Cache the image once it's loaded
            imageCache[imagePath] = img;
            
            //resolve(img); // Resolve the promise once the image is loaded

            try {
                // Fetch the image as ArrayBuffer (binary data)
                const response = await fetch(img.src);
                const arrayBuffer = await response.arrayBuffer();

                //const base64String = arrayBufferToBase64(arrayBuffer);

                const base64String = btoa(String.fromCharCode(...new Uint8Array(arrayBuffer)));
                resolve(base64String); // Resolve the promise with the image data (ArrayBuffer)
                console.log("preloaded");
            } catch (error) {
                reject("Failed to load image data: " + error);
            }

        };

        img.onerror = (error) => {
            reject("Failed to load image: " + imagePath);
        };
        
        
    });
}

function drawOnUi(x, y, img, width,height,flipHorizontally = false) {
    uiCtx.save();
    if (flipHorizontally) {
        uiCtx.scale(-1, 1); // Flip the image horizontally
        uiCtx.drawImage(imageCache["assets/Ui/" + img], -x - width, y, width, height);
    }
    else {
        uiCtx.drawImage(imageCache["assets/Ui/" + img], x, y, width, height);
    }
    uiCtx.restore();
}

function finalDraw()
{
    ctx.drawImage(offscreenCanvas, 0, 0);  
}






function drawAll(listOfUnits) {
    clearCanvas();
    //units = JSON.parse(listOfUnits);
    //units.forEach(unit => {

    //    draw(unit.X, unit.Y, unit.animationToDraw, unit.flipAnimation, unit.width, unit.height, unit.rotate)
    //});
    units = JSON.parse(listOfUnits);
    return new Promise((resolve) => {
        //clearCanvas();
        units.forEach(unit => {
            draw(unit.X, unit.Y, unit.animationToDraw, unit.flipAnimation, unit.width, unit.height, unit.rotate)
        });
        resolve();
    });

    
    
    //finalDraw();
}

function Victory() {
    window.alert('Victory');
}
function Defeat() {
    window.alert('Defeat');
}

function draw(x, y, img, flipHorizontally=false,width=0,height=0,rotate=0) {

    ctx.save(); // Save the current context state
    
    tmpImg = imageCache["assets/" + img];

    if (!tmpImg) {
        ctx.fillText(img, x, y);
        return;
    }

    tmpHeight = tmpImg.height;
    tmpWidth = tmpImg.width;
    if (width != 0 && height != 0)
    {
        tmpHeight = height;
        tmpWidth = width;
    }

    if (rotate != 0)
    {
        // Translate to the center of the image
        ctx.translate(x + tmpWidth / 2, y + tmpHeight / 2);

        // Rotate the canvas by the desired angle (in radians)
        ctx.rotate(rotate);

        if (flipHorizontally) {
            ctx.scale(-1, 1); // Flip the image horizontally
        }
        // Draw the image, offset by half its width and height to keep it centered
        ctx.drawImage(tmpImg, -tmpWidth / 2, -tmpHeight / 2, tmpWidth, tmpHeight);
        ctx.restore();
        return;
    }

    if (flipHorizontally) {
        ctx.scale(-1, 1); // Flip the image horizontally

        ctx.drawImage(tmpImg, -x - tmpWidth, y, tmpWidth, tmpHeight);
    }
    else {

        ctx.drawImage(tmpImg, x, y, tmpWidth, tmpHeight);
    }
    ctx.restore(); // Restore the original state

    //console.log(tmpImg)
    
}


// Get the canvas position relative to the viewport
const canvasRect = canvas.getBoundingClientRect();

// Calculate the x and y offset
const offsetX = canvasRect.left;
const offsetY = canvasRect.top;
window.addEventListener('click', function (event) {
    // Get the position of the click
    const x = event.clientX - offsetX;
    const y = event.clientY;

    DotNet.invokeMethodAsync('game', 'HandleClick', x, y);
    //spawnAlly('Bat');
});

// Continuous key state handling
window.addEventListener("keydown", function (event) {
    if (!keyStates[event.key]) {
        keyStates[event.key] = true;
        DotNet.invokeMethodAsync('game', 'HandleKeyDown', event.key);
    }
    //spawnEnemy('Bat');
});

window.addEventListener("keyup", function (event) {
    keyStates[event.key] = false;
    DotNet.invokeMethodAsync('game', 'HandleKeyUp', event.key);
});


function spawnEnemy(enemyUnit) {
    DotNet.invokeMethodAsync('game', 'SpawnEnemy', enemyUnit);
}

function spawnAlly(allyUnit) {
    DotNet.invokeMethodAsync('game', 'SpawnAlly', allyUnit);
}

function canBuyUnit(allyUnit) {
    // Invoke the .NET method and handle the result
    return DotNet.invokeMethodAsync('game', 'CanBuy', allyUnit)
        .then(result => {
            // `result` will be the boolean value returned from the .NET method
            console.log(`Can buy ${allyUnit}: ${result}`);
            return result; // Return the boolean value
        })
        .catch(error => {
            console.error(`Error calling CanBuy: ${error}`);
            return false; // Handle errors by returning false or another default value
        });
}