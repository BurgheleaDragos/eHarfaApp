window.eHarfa = window.eHarfa || {};

window.eHarfa.navigateBack = (fallbackUrl) => {
    if (window.history.length > 1) {
        window.history.back();
        return;
    }

    window.location.assign(fallbackUrl);
};

window.eHarfa.copyToClipboard = async (text) => {
    try {
        if (navigator.clipboard && window.isSecureContext) {
            await navigator.clipboard.writeText(text);
            return true;
        }

        const textArea = document.createElement("textarea");
        textArea.value = text;
        textArea.setAttribute("readonly", "");
        textArea.style.position = "fixed";
        textArea.style.opacity = "0";
        document.body.appendChild(textArea);
        textArea.select();
        const copied = document.execCommand("copy");
        textArea.remove();
        return copied;
    } catch {
        return false;
    }
};

function base64ToBytes(base64Content) {
    const binary = atob(base64Content);
    const bytes = new Uint8Array(binary.length);

    for (let index = 0; index < binary.length; index += 1) {
        bytes[index] = binary.charCodeAt(index);
    }

    return bytes;
}

function downloadPdfBytes(fileName, bytes) {
    const blob = new Blob([bytes], { type: "application/pdf" });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    anchor.remove();
    URL.revokeObjectURL(url);
}

window.eHarfa.downloadPdfFromBase64 = (fileName, base64Content) => {
    downloadPdfBytes(fileName, base64ToBytes(base64Content));
};

window.eHarfa.shareSongPdf = async (fileName, base64Content, title) => {
    const bytes = base64ToBytes(base64Content);
    const file = new File([bytes], fileName, { type: "application/pdf" });

    if (navigator.canShare && navigator.canShare({ files: [file] })) {
        try {
            await navigator.share({ files: [file], title });
            return "shared";
        } catch (error) {
            if (error && error.name === "AbortError") {
                return "cancelled";
            }
            downloadPdfBytes(fileName, bytes);
            return "fallback";
        }
    }

    downloadPdfBytes(fileName, bytes);
    return "fallback";
};
