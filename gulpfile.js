var gulp = require('gulp'),

    concat = require('gulp-concat'),
    foreach = require('gulp-foreach'),
    gutil = require('gulp-util'),
    fs = require("fs"),
    rename = require('gulp-rename'),
    merge = require('merge'),

    handlebars = require('gulp-compile-handlebars'),

    browserSync = require('browser-sync').create(),

    cssmin = require('gulp-cssmin'),
    sourcemaps = require('gulp-sourcemaps'),
    autoprefixer = require('gulp-autoprefixer'),
    sass = require('gulp-sass'),

    imageResize = require('gulp-image-resize'),
    imagemin = require('gulp-imagemin'),

    browserify = require('browserify'),
    jsmin = require('gulp-jsmin')
    babelify = require('babelify'),
    source = require('vinyl-source-stream'),
    buffer = require('vinyl-buffer');

var config = {
    dist: "dist"
}

gulp.task('copy', function () {
    gulp.src('src/plugins/animate/*.css')
        .pipe(gulp.dest(config.dist + "/css"));

    gulp.src('src/plugins/bootstrap/css/*.min.css')
        .pipe(gulp.dest(config.dist + "/css"));

    gulp.src('src/plugins/bootstrap/js/*.min.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/plugins/jquery/*.min.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/plugins/owl-carousel/*.min.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/plugins/owl-carousel/assets/*.min.css')
        .pipe(gulp.dest(config.dist + "/css"));

    gulp.src('src/plugins/owl-carousel/assets/*.{gif,png}')
        .pipe(gulp.dest(config.dist + "/css"));

    gulp.src('src/plugins/pe-icons/**/*.*')
        .pipe(gulp.dest(config.dist + "/css"));

    gulp.src('src/plugins/video-background/*.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/plugins/video-background/*.css')
        .pipe(gulp.dest( config.dist + "/css"));

    gulp.src('src/plugins/flexslider/*-min.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/plugins/flexslider/*.css')
        .pipe(gulp.dest( config.dist + "/css"));

    gulp.src('src/plugins/flexslider/fonts/*.*')
        .pipe(gulp.dest( config.dist + "/css/fonts"));

    gulp.src('src/plugins/font-awesome/css/*.min.css')
        .pipe(gulp.dest( config.dist + "/css"));
    
    gulp.src('src/plugins/font-awesome/fonts/*.*')
        .pipe(gulp.dest( config.dist + "/fonts"));

    gulp.src('src/plugins/wow/dist/*.min.js')
        .pipe(gulp.dest( config.dist + "/js"));

    gulp.src('src/images/**/*.*')
        .pipe(gulp.dest( config.dist + "/images"));
});

gulp.task('scss', ['copy'], function () {
    return gulp.src('src/data/**/*.json')
        .pipe(foreach(function (stream, file) {
            var json = JSON.parse(fs.readFileSync(file.path, "utf8"));
            return gulp.src('src/scss/pages/' + json.page + '.scss')
                .pipe(sass().on('error', sass.logError))
                .pipe(cssmin())
                .pipe(gulp.dest(config.dist + '/css'))
                .pipe(browserSync.stream());
        }));
});

gulp.task('js', function () {
    return browserify('src/js/app.js', { debug: true })
        .bundle()
        .pipe(source('app.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        //.pipe(jsmin())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(config.dist + '/js'))
        .pipe(browserSync.stream());
});

gulp.task('fonts', function () {
    gulp.src('src/bootstrap/fonts/*.*')
        .pipe(gulp.dest( config.dist + "/fonts"));
});

gulp.task('html', function () {
    var options = {
        ignorePartials: true
    }

    var mixinsJson = {};
    var mixins = [];

    for (var i = 0; i < mixins.length; i++) {
        merge.recursive(mixinsJson, JSON.parse(fs.readFileSync('src/data/mixins/' + mixins[i] + '.json', "utf8")));
    }

    return gulp.src('src/data/pages/**/*.json')
        .pipe(foreach(function (stream, file) {
            var json = JSON.parse(fs.readFileSync(file.path, "utf8"));
            var data = merge.recursive(json, mixinsJson);
            return gulp.src('src/html/pages/' + json.page + '.hbs')
                .pipe(handlebars(data, options))
                .pipe(rename('index.html'))
                .pipe(gulp.dest(json.output));
        }));
});

gulp.task('run', function () {
    browserSync.init({
        server: {
            baseDir: config.dist
        }
    });

    gulp.watch("src/scss/**/*.scss", ['scss']);
    gulp.watch("src/js/**/*.js", ['js']);
    gulp.watch("src/data/**/*.json", ['html', 'sass']);
    gulp.watch("src/html/**/*.hbs", ['html']);
    gulp.watch(config.dist + "/**/*.html").on('change', browserSync.reload);
    gulp.watch(config.dist + "/js/*.js").on('change', browserSync.reload);
});

gulp.task('build', ['scss', 'js', 'fonts', 'html']);