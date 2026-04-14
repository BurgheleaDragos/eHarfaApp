window.eHarfa = window.eHarfa || {};

window.eHarfa.downloadPdfFromBase64 = (fileName, base64Content) => {
    const binary = atob(base64Content);
    const bytes = new Uint8Array(binary.length);

    for (let index = 0; index < binary.length; index += 1) {
        bytes[index] = binary.charCodeAt(index);
    }

    const blob = new Blob([bytes], { type: "application/pdf" });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    anchor.remove();
    URL.revokeObjectURL(url);
};
