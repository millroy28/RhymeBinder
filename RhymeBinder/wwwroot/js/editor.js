//Chaning tab behavior in text editor window so instead of changing focus we're insering a... tab character
let input = document.getElementById('body_edit_field');

input.addEventListener("keydown", (e) => {
    if (e.key == "Tab") {
        document.execCommand('insertHTML', false, '&#009');
        e.preventDefault();
    } 
});


//Code for drawers

var drawer = function () {

    /**
    * Element.closest() polyfill
    * https://developer.mozilla.org/en-US/docs/Web/API/Element/closest#Polyfill
    * https://www.cssscript.com/easy-sliding-drawer/
    */
    if (!Element.prototype.closest) {
        if (!Element.prototype.matches) {
            Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;
        }
        Element.prototype.closest = function (s) {
            var el = this;
            var ancestor = this;
            if (!document.documentElement.contains(el)) return null;
            do {
                if (ancestor.matches(s)) return ancestor;
                ancestor = ancestor.parentElement;
            } while (ancestor !== null);
            return null;
        };
    }


    //
    // Settings
    //
    var settings = {
        speedOpen: 50,
        speedClose: 350,
        activeClass: 'is-active',
        visibleClass: 'is-visible',
        selectorTarget: '[data-drawer-target]',
        selectorTrigger: '[data-drawer-trigger]',
        selectorClose: '[data-drawer-close]',

    };


    //
    // Methods
    //

    // Toggle accessibility
    var toggleccessibility = function (event) {
        if (event.getAttribute('aria-expanded') === 'true') {
            event.setAttribute('aria-expanded', false);
        } else {
            event.setAttribute('aria-expanded', true);
        }
    };

    // Open Drawer
    var openDrawer = function (trigger) {

        // Find target
        var target = document.getElementById(trigger.getAttribute('aria-controls'));

        // Make it active
        target.classList.add(settings.activeClass);

        // Make body overflow hidden so it's not scrollable
        document.documentElement.style.overflow = 'hidden';

        // Toggle accessibility
        toggleccessibility(trigger);

        // Make it visible
        setTimeout(function () {
            target.classList.add(settings.visibleClass);
        }, settings.speedOpen);

    };

    // Close Drawer
    var closeDrawer = function (event) {

        // Find target
        var closestParent = event.closest(settings.selectorTarget),
            childrenTrigger = document.querySelector('[aria-controls="' + closestParent.id + '"');

        // Make it not visible
        closestParent.classList.remove(settings.visibleClass);

        // Remove body overflow hidden
        document.documentElement.style.overflow = '';

        // Toggle accessibility
        toggleccessibility(childrenTrigger);

        // Make it not active
        setTimeout(function () {
            closestParent.classList.remove(settings.activeClass);
        }, settings.speedClose);

    };

    // Click Handler
    var clickHandler = function (event) {

        // Find elements
        var toggle = event.target,
            open = toggle.closest(settings.selectorTrigger),
            close = toggle.closest(settings.selectorClose);

        // Open drawer when the open button is clicked
        if (open) {
            openDrawer(open);
        }

        // Close drawer when the close button (or overlay area) is clicked
        if (close) {
            closeDrawer(close);
        }

        // Prevent default link behavior
        if (open || close) {
            event.preventDefault();
        }

    };

    // Keydown Handler, handle Escape button
    var keydownHandler = function (event) {

        if (event.key === 'Escape' || event.keyCode === 27) {

            // Find all possible drawers
            var drawers = document.querySelectorAll(settings.selectorTarget),
                i;

            // Find active drawers and close them when escape is clicked
            for (i = 0; i < drawers.length; ++i) {
                if (drawers[i].classList.contains(settings.activeClass)) {
                    closeDrawer(drawers[i]);
                }
            }

        }

    };


    //
    // Inits & Event Listeners
    //
    document.addEventListener('click', clickHandler, false);
    document.addEventListener('keydown', keydownHandler, false);


};

drawer();

//Code for LineCounter
function get_characters_per_line() {
    var textBox = document.getElementById('body_edit_field');
    var checkBox = document.getElementById('textWidthSample');
    var textBoxWidth = textBox.scrollWidth;

    checkBox.hidden = false;
    var textCharacterWidth = checkBox.clientWidth;
    checkBox.hidden = true;

    var estCharactersInLine = Math.floor(textBoxWidth / textCharacterWidth);
    console.log('Estimated Characters per Line: ' + estCharactersInLine);
    return estCharactersInLine;
}
function generate_line_display_string() {
    var charsInLine = get_characters_per_line();
    var text = document.getElementById('body_edit_field').value;

    var splitTextArray = text.split("\n");
    var splitTextArrayCount = splitTextArray.length;
    var arrayLine;
    var arrayLineLength;
    var lineDisplayString = "";
    var textLineNumber = 1;

    for (let i = 0; i < splitTextArrayCount; i++) {
        
        let arrayLine = splitTextArray[i];
        let arrayLineLength = arrayLine.length;
        //If arrayline has no characters besides spaces, it is not a line
        //do with regex at some point?

        console.log("textarea row: " + i + "; row length: " + arrayLineLength + "; max row length: " + charsInLine );

        //if text overruns line, don't count subsequent lines
        if (arrayLineLength > charsInLine) {
            console.log('overrun line at ' + i);
            let overRunFactor = Math.floor(arrayLineLength / charsInLine);
            lineDisplayString += textLineNumber.toString();
            textLineNumber++;
            
            
            //add extra line breaks so next line number matches next populated line in text area
            for (let j = 0; j < overRunFactor; j++) {
                lineDisplayString += ' \r\n';
            }

        }
        if (arrayLineLength > 0 && arrayLineLength <= charsInLine) {
            console.log('line exists at ' + i);
            lineDisplayString += textLineNumber.toString();
            textLineNumber++;
        }


        lineDisplayString += ' \r\n';

    };

    return lineDisplayString;
}
function populate_line_count() {
    let lineCount = generate_line_display_string();
    document.getElementById('line_count').value = lineCount;
}
populate_line_count(); //do it once on load

window.addEventListener("resize", populate_line_count);
let textboxTyping = document.getElementById('body_edit_field');
input.addEventListener('keyup', function (e) {
    populate_line_count();
});

