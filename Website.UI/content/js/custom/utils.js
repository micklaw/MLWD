﻿var utils = (function () {

    var request = (function () {

        var doPost = function (url, data, callback) {

            $.ajax({
                type: 'post',
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                cache: false
            })
                .done(function (response) {
                    if (typeof (callback) == "function") {
                        callback(response);
                    }
                });
        };

        var complete = function (gameId, taskId, score, callback) {

            var data = {
                GameId: gameId,
                TaskId: taskId,
                Score: score,
                PlayerId: window.jsBag.user.id
            };

            $.ajax({
                type: 'post',
                url: '/api/tasks/complete',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                cache: false
            })
                .done(function (response) {
                    if (typeof (callback) == "function") {
                        callback(response);
                    }
                });
        };

        var json = function (url, data, callback) {
            $.getJSON(url, data)
                .done(function (response) {
                    if (typeof (callback) == "function") {
                        callback(response);
                    }
                });
        };

        var preventJacking = function () {
            try {
                top.document.domain;
            } catch (e) {
                var f = function () {
                    document.body.innerHTML = '';
                };

                setInterval(f, 1);

                if (document.body)
                    document.body.onload = f;
            }

        };

        return {
            post: doPost,
            json: json,
            complete: complete,
            jack: preventJacking
        };

    })();

    var validate = (function () {

        var onErrors = function (validator, form) {
            var $form = $(form);
            var container = $form.find("[data-valmsg-summary]"),
                list = container.find("ul");

            if (list && list.length && validator.errorList.length) {
                list.empty();
                container.addClass("validation-summary-errors").removeClass("validation-summary-valid");
                $.each(validator.errorList, function () {
                    $("<li />").html(this.message).appendTo(list);
                });
            }
        };

        var errorPlacement = function (error, element) {

        };

        var submitOnce = function() {

            $("form[submitonce]").each(function () {
                var $form = $(this);
                
                $form.bind('invalid-form.validate', function () {
                    var $button = $(this).find('input[type="submit"],button');

                    setTimeout(function () {
                        $button.removeAttr('disabled');
                    }, 1);
                });

                $form.on('submit', function () {
                    var $button = $(this).find('input[type="submit"],button');
                    
                    setTimeout(function () {
                        $button.attr('disabled', 'disabled');
                    }, 0);
                });
            });
            
        };

        return {
            render: onErrors,
            placement: errorPlacement,
            submitOnce: submitOnce
        };

    })();

    var date = (function () {

        var since = function (dateSince) {
            var date1 = new Date((dateSince.split('.')[0] || "").replace(/-/g, "/").replace(/[TZ]/g, " ")),
		diff = (((new Date()).getTime() - date1.getTime()) / 1000),
		day_diff = Math.floor(diff / 86400);

            if (isNaN(day_diff) || day_diff < 0 || day_diff >= 31)
                return;

            return day_diff == 0 && (
                    diff < 60 && "just now" ||
                    diff < 120 && "1 minute ago" ||
                    diff < 3600 && Math.floor(diff / 60) + " minutes ago" ||
                    diff < 7200 && "1 hour ago" ||
                    diff < 86400 && Math.floor(diff / 3600) + " hours ago") ||
                day_diff == 1 && "Yesterday" ||
                day_diff < 7 && day_diff + " days ago" ||
                day_diff < 31 && Math.ceil(day_diff / 7) + " weeks ago";
        };

        return {
            since: since
        };

    })();

    var design = (function () {
        var doEqualise = function(element) {
            equalise(element);

            var equaliseTimer;
            $(window).resize(function() {
                clearTimeout(equaliseTimer);
                equaliseTimer = setTimeout(equalise(element), 500);
            });
        };
        
        var equalise = function (element) {
            $(window).load(function () {
                var maxHeight = 0;

                var $elements = $(element);

                if ($elements.length > 0) {
                    $elements.each(function () {
                        if ($(this).height() > maxHeight) {
                            maxHeight = $(this).height();
                        }
                    });

                    $elements.height(maxHeight);
                }
            });
        };

        var cleanYoutube = function () {
            var iframes = document.getElementsByTagName('iframe');

            for (var i = 0; i < iframes.length; i++) {
                if (iframes[i].src.indexOf("youtube") != -1) {
                    var seperator = (iframes[i].src.indexOf("?") == -1) ? "?" : "&";

                    iframes[i].src += seperator + "wmode=transparent&rel=0&modestbranding=1";
                }
            }
        };

        var fluidVideo = function (container) {
            var $wrapper = $(container);

            if ($wrapper.length > 0) {
                var $allVideos = $("iframe[src*='vimeo.com/'], iframe[src*='youtube.com/'], object, embed");

                if ($allVideos.length > 0) {
                    $allVideos.each(function () {
                        $(this).attr('data-aspectRatio', this.height / this.width)
                            .removeAttr('height')
                            .removeAttr('width');
                    });

                    $(window).resize(function () {
                        var newWidth = $wrapper.width();

                        $allVideos.each(function () {
                            var $el = $(this);

                            $el.width(newWidth).height(newWidth * $el.attr('data-aspectRatio'));
                        });

                    }).resize();
                }
            }
        };

        var tabs = function (content, nav) {

            $(window).load(function () {
                var $content = $(content);

                if ($content.length > 0) {

                    var currentIndex = 0;

                    var $nav = $(nav);

                    if ($nav.length > 0) {

                        $content.css("min-height", $content.height());

                        $('a', $nav).on("click", function () {
                            var index = $(this).index();

                            if (index != currentIndex) {
                                currentIndex = index;

                                var $toShow = $("> *", $content).eq(index);

                                var containerHeight = $toShow.outerHeight();

                                $("> *", $content).hide();

                                $content.animate({ "min-height": containerHeight }, 300, function () {
                                    $toShow.fadeIn(1000);
                                });
                            }
                        });

                    }
                }

            });
        };

        var placeholder = function () {

            if (!features.placeholder()) {
                $("input").each(function () {
                    var $this = $(this);

                    if ($this.val() == "" && $this.attr("placeholder") != "") {

                        $this.val($this.attr("placeholder"));

                        $this.focus(function () {
                            if ($this.val() == $this.attr("placeholder"))
                                $this.val("");
                        });

                        $this.blur(function () {
                            if ($this.val() == "")
                                $this.val($this.attr("placeholder"));
                        });

                    }
                });
            }
        };

        var loadImages = function () {

            var noScript = $("noscript[data-lazy-image]");

            console.log(noScript.length);
            
            if (noScript.length > 0) {
                noScript.each(function () {
                    createImage(this);
                });
            }

            function updateQueryStringParameter(uri, key, value) {
                var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
                var separator = uri.indexOf('?') !== -1 ? "&" : "?";
                if (uri.match(re)) {
                    return uri.replace(re, '$1' + key + "=" + value + '$2');
                }
                else {
                    return uri + separator + key + "=" + value;
                }
            }
            
            function createImage(element) {
                var $noScript = $(element);

                var src = getUrl($noScript);

                var $img = $("<img data-lazy-image />");
                $img.attr("class", $noScript.attr("class"));
                $img.attr("alt", $noScript.attr("data-alt"));
                $img.attr("data-actual", $noScript.attr("data-actual"));
                $img.attr("data-actual-height", $noScript.attr("data-actual-height"));
                $img.attr("data-src", $noScript.attr("data-src"));
                $img.attr("src", src);

                $noScript.after($img);
            }

            function getUrl(element) {
                var $element = $(element);
                var $parent = $element.parents(":visible:first");

                var width = $parent.width();
                var url = $element.attr("data-src");
                var maxWidth = parseInt($element.attr("data-actual"));
                var maxHeight = parseInt($element.attr("data-actual-height"));

                var resizeWidth = maxWidth;
                var resizeHeight = maxHeight;
                
                if (width < maxWidth) {
                    var ratio = width / maxWidth;

                    resizeWidth = width;
                    resizeHeight = maxHeight * ratio;
                }

                url = updateQueryStringParameter(url, "width", resizeWidth);
                url = updateQueryStringParameter(url, "height", resizeHeight);

                return url;
            }

            function resizeImage(img) {
                var $img = $(img);
                var src = getUrl($img);

                $img.attr("src", src);
            }
            function resizeImages() {
                var $lazy = $("img[data-lazy-image]");

                if ($lazy.length > 0) {
                    $lazy.each(function () {
                        resizeImage(this);
                    });
                }
            }

            var doImage;
            $(window).resize(function () {
                clearTimeout(doImage);
                doImage = setTimeout(resizeImages, 200);
            });

        };
            
        return {
            equalise: doEqualise,
            fluidVideo: fluidVideo,
            cleanYoutube: cleanYoutube,
            tabs: tabs,
            loadImages: loadImages,
            placeholder: placeholder
        };

    })();

    var array = (function () {

        /**
        * Randomize array element order in-place.
        * Using Fisher-Yates shuffle algorithm.
        */
        var shuffle = function (array) {
            for (var i = array.length - 1; i > 0; i--) {
                var j = Math.floor(Math.random() * (i + 1));
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return array;
        }

        return {
            shuffle: shuffle
        };

    })();

    var features = (function () {

        var placeholder = function () {
            return document.createElement("input").placeholder != undefined;
        };

        return {
            placeholder: placeholder
        };

    })();

    return {
        request: request,
        validate: validate,
        date: date,
        design: design,
        array: array
    };

})();