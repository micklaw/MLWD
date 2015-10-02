var global = (function () {
    utils.design.loadImages();
    console.log("images");
    utils.design.placeholder();
    utils.design.equalise("*[data-equalise]");
    utils.design.fluidVideo("*[data-video-wrapper]");
    utils.design.cleanYoutube();
    utils.request.jack();
    utils.validate.submitOnce();
})();