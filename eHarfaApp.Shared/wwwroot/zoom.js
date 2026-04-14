window.eHarfa = window.eHarfa || {};

window.eHarfa.initZoom = (className, dotNetRef) => {
    const el = document.querySelector(`.${className}`);
    if (!el || !window.Hammer) return;

    const minFontSize = 0.7;
    const maxFontSize = 2.5;
    let currentFontSize = parseFloat(getComputedStyle(el).fontSize) / 16;
    let startFontSize = currentFontSize;

    const hammer = new Hammer(el, { touchAction: 'pan-y' });
    hammer.get('pinch').set({ enable: true });

    hammer.on('pinchstart', () => {
        startFontSize = currentFontSize;
    });

    hammer.on('pinchend', (e) => {
        currentFontSize = Math.min(maxFontSize, Math.max(minFontSize, startFontSize * e.scale));
        dotNetRef.invokeMethodAsync('OnFontSizeChanged', currentFontSize);
    });
};
