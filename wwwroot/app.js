// Auto-grow textarea based on content
function autoGrowTextarea(element) {
    if (element) {
        element.style.height = "auto";
        element.style.height = Math.min(element.scrollHeight, 300) + "px";
    }
}

// Scroll to bottom of an element
function scrollToBottom(element) {
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
}

// Copy text to clipboard
function copyText(text) {
    navigator.clipboard.writeText(text).then(() => {
        console.log("Text copied to clipboard");
    }).catch(err => {
        console.error("Failed to copy text:", err);
    });
}

// Share text using the Web Share API
function shareText(title, text) {
    if (navigator.share) {
        navigator.share({
            title: title,
            text: text
        }).catch(err => {
            console.error("Error sharing:", err);
        });
    } else {
        console.log("Web Share API not supported, falling back to copy");
        copyText(text);
    }
}

// Get currently selected text
function getSelectedText() {
    return window.getSelection().toString();
}

// Focus an element
function focusElement(element) {
    if (element) {
        element.focus();
    }
}
