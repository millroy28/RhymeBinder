function set_sample_colors() {
    var colorValue = document.getElementById('color').value;
    var newFontColor = calc_font_color(colorValue);
    // do a check here to see if color changed
    var newBackgroundColor = calc_background_color(colorValue);
    var newSelectionColor = calc_selection_color(colorValue);

    console.log("User selected color " + colorValue);
    console.log("Derived font color " + newFontColor);
    console.log("Derived background color " + newBackgroundColor);
    console.log("Derived selection color " + newSelectionColor);

    // set hidden fields which will be sent to back end on form submission
    document.getElementsByName('MainColor').value = colorValue;
    document.getElementsByName('FontColor').value = newFontColor;
    document.getElementsByName('BackgroundColor').value = newBackgroundColor;
    document.getElementsByName('SelectionColor').value = newSelectionColor;

    // set colors in sample area
    document.getElementById('sampleMenuBar').style.backgroundColor = colorValue;

    var dropdowns = document.getElementsByName('sampleDropDown');
    dropdowns.forEach((element) => { element.style.color = newFontColor; });

    var buttons = document.getElementsByName('sampleButton');
    buttons.forEach((element) => {
        element.style.color = newFontColor;
        // element.stlye.backgroundColor = newBackgroundColor;
    });

    document.getElementById('sampleBody').style.backgroundColor = newBackgroundColor;



}

function calc_font_color(selectedColor) {
    // if selected color is dark, set font color to white.
    // ...and vice versa

    var fontColor;
    var luminance = calc_luminance(selectedColor);

    fontColor = luminance > 0.179 ? '#000000' : '#FFFFFF';

    return fontColor;

}
function calc_background_color(selectedColor) {
    // set background color to a percentage shade lighter
    backgroundColor = calc_shifted_color(selectedColor, -50);
    return backgroundColor;
}

function calc_selection_color(selectedColor) {
    // set selection color to a percentage shade darker
    selectedColor = calc_shifted_color(selectedColor, -90);
    return selectedColor;
}

function calc_luminance(selectedColor) {
    // return ratio for contrast

    // get int r/g/b values:
    selectedColor = selectedColor.replace(/\#/g, '');

    var red = parseInt(selectedColor.substring(0, 2), 16);
    var green = parseInt(selectedColor.substring(2, 4), 16);
    var blue = parseInt(selectedColor.substring(4, 6), 16);

    // from https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color
    var colors = [red / 255, green / 255, blue / 255];
    var c = colors.map((col) => {
        if (col <= 0.03928) {
            return col / 12.92;
        }
        return Math.pow((col + 0.055) / 1.055, 2.4);
    });

    var L = (0.2126 * c[0]) + (0.7152 * c[1]) + (0.0722 * c[2]);

    return L;
}

function calc_shifted_color(selectedColor, amount) {
    // shift color brigter or darker a certain amount

    // get int r/g/b values:
    selectedColor = selectedColor.replace(/\#/g, '');

    var red = parseInt(selectedColor.substring(0, 2), 16);
    var green = parseInt(selectedColor.substring(2, 4), 16);
    var blue = parseInt(selectedColor.substring(4, 6), 16);

    red = red + amount;
    green = green + amount;
    blue = blue + amount;

    red = red > 255 ? 255 : red;
    red = red < 0 ? 0 : red;
    green = green > 255 ? 255 : green;
    green = green < 0 ? 0 : green;
    blue = blue > 255 ? 255 : blue;
    blue = blue < 0 ? 0 : blue;

    // convert back to RGB

    redHex = red.toString(16);
    greenHex = green.toString(16);
    blueHex = blue.toString(16);

    newColor = '#' + redHex + greenHex + blueHex;
    return newColor;

}


// from KFox112 on StackOverflow:
// https://stackoverflow.com/questions/11023144/working-with-hex-strings-and-hex-values-more-easily-in-javascript
function shiftColor(base, change, direction) {
    const colorRegEx = /^\#?[A-Fa-f0-9]{6}$/;

    // Missing parameter(s)
    if (!base || !change) {
        return '000000';
    }

    // Invalid parameter(s)
    if (!base.match(colorRegEx) || !change.match(colorRegEx)) {
        return '000000';
    }

    // Remove any '#'s
    base = base.replace(/\#/g, '');
    change = change.replace(/\#/g, '');

    // Build new color
    let newColor = '';
    for (let i = 0; i < 3; i++) {
        const basePiece = parseInt(base.substring(i * 2, i * 2 + 2), 16);
        const changePiece = parseInt(change.substring(i * 2, i * 2 + 2), 16);
        let newPiece = '';

        if (direction === 'add') {
            newPiece = (basePiece + changePiece);
            newPiece = newPiece > 255 ? 255 : newPiece;
        }
        if (direction === 'sub') {
            newPiece = (basePiece - changePiece);
            newPiece = newPiece < 0 ? 0 : newPiece;
        }

        newPiece = newPiece.toString(16);
        newPiece = newPiece.length < 2 ? '0' + newPiece : newPiece;
        newColor += newPiece;
    }

    return newColor;
}