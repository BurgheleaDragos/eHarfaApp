window.eHarfa = window.eHarfa || {};

window.eHarfa.initSongGestures = (className, textToCopy, dotNetRef) => {
    const el = document.querySelector(`.${className}`);
    if (!el) return;

    window.eHarfa.disposeSongGestures(className);

    const minFontSize = 0.7;
    const maxFontSize = 2.5;
    const rootFontSize = parseFloat(getComputedStyle(document.documentElement).fontSize) || 16;
    let pinchStartDistance = 0;
    let pinchStartFontSize = 1;
    let lastReportedFontSize = 1;
    let animationFrame = 0;
    let pendingFontSize = 1;
    let longPressStart = null;

    const getDistance = (firstTouch, secondTouch) => Math.hypot(
        secondTouch.clientX - firstTouch.clientX,
        secondTouch.clientY - firstTouch.clientY);

    const reportFontSize = (fontSize, immediately = false) => {
        pendingFontSize = Math.min(maxFontSize, Math.max(minFontSize, fontSize));
        if (!immediately && Math.abs(pendingFontSize - lastReportedFontSize) < 0.01) return;

        const update = () => {
            animationFrame = 0;
            lastReportedFontSize = pendingFontSize;
            dotNetRef.invokeMethodAsync('OnFontSizeChanged', pendingFontSize);
        };

        if (immediately) {
            if (animationFrame) cancelAnimationFrame(animationFrame);
            update();
        } else if (!animationFrame) {
            animationFrame = requestAnimationFrame(update);
        }
    };

    const onTouchStart = (event) => {
        if (event.touches.length === 2) {
            pinchStartDistance = getDistance(event.touches[0], event.touches[1]);
            pinchStartFontSize = parseFloat(getComputedStyle(el).fontSize) / rootFontSize;
            lastReportedFontSize = pinchStartFontSize;
            longPressStart = null;
            return;
        }

        if (event.touches.length === 1) {
            const touch = event.touches[0];
            longPressStart = { time: Date.now(), x: touch.clientX, y: touch.clientY, moved: false };
        }
    };

    const onTouchMove = (event) => {
        if (event.touches.length === 2 && pinchStartDistance > 0) {
            event.preventDefault();
            const distance = getDistance(event.touches[0], event.touches[1]);
            reportFontSize(pinchStartFontSize * (distance / pinchStartDistance));
            return;
        }

        if (longPressStart && event.touches.length === 1) {
            const touch = event.touches[0];
            if (Math.hypot(touch.clientX - longPressStart.x, touch.clientY - longPressStart.y) > 12) {
                longPressStart.moved = true;
            }
        }
    };

    const onTouchEnd = async (event) => {
        if (pinchStartDistance > 0) {
            if (event.touches.length < 2) {
                reportFontSize(pendingFontSize, true);
                pinchStartDistance = 0;
            }
            return;
        }

        if (longPressStart && !longPressStart.moved && Date.now() - longPressStart.time >= 550) {
            event.preventDefault();
            const copied = await window.eHarfa.copyToClipboard(textToCopy);
            dotNetRef.invokeMethodAsync('OnSongCopied', copied);
        }

        longPressStart = null;
    };

    const onTouchCancel = () => {
        pinchStartDistance = 0;
        longPressStart = null;
    };

    const onContextMenu = (event) => event.preventDefault();

    el.addEventListener('touchstart', onTouchStart, { passive: true });
    el.addEventListener('touchmove', onTouchMove, { passive: false });
    el.addEventListener('touchend', onTouchEnd, { passive: false });
    el.addEventListener('touchcancel', onTouchCancel, { passive: true });
    el.addEventListener('contextmenu', onContextMenu);

    el.__eHarfaSongGestureCleanup = () => {
        if (animationFrame) cancelAnimationFrame(animationFrame);
        el.removeEventListener('touchstart', onTouchStart);
        el.removeEventListener('touchmove', onTouchMove);
        el.removeEventListener('touchend', onTouchEnd);
        el.removeEventListener('touchcancel', onTouchCancel);
        el.removeEventListener('contextmenu', onContextMenu);
        delete el.__eHarfaSongGestureCleanup;
    };
};

window.eHarfa.disposeSongGestures = (className) => {
    const el = document.querySelector(`.${className}`);
    el?.__eHarfaSongGestureCleanup?.();
};
