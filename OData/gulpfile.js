/// <binding BeforeBuild='default' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var csso = require('gulp-csso');
var rimraf = require("rimraf");
var merge = require('merge-stream');

gulp.task("minify-js", function () {

    var streams = [
        gulp.src(["wwwroot/js/*.js"])
            .pipe(uglify())
            .pipe(concat("site.min.js"))
            .pipe(gulp.dest("wwwroot/lib/site/js"))
    ];

    return merge(streams);
});

gulp.task("minify-css", function () {

    return gulp.src('wwwroot/css/site.css')
        .pipe(csso())
        .pipe(concat("site.min.css"))
        .pipe(gulp.dest('wwwroot/lib/site/css'));
});

gulp.task("minify", gulp.parallel('minify-js', 'minify-css'));

// Dependency Dirs
var deps = {
    "bootstrap": {
        "dist/**/*": ""
    },
    "bootstrap-table": {
        "dist/**/*": ""
    },
    "datatables": {
        "media/**/*": ""
    },
    "jquery": {
        "dist/*": ""
    },
    "popper.js": {
        "dist/**/*": ""
    }
};

gulp.task("clean-vendor", function (cb) {
    return rimraf("wwwroot/vendor/", cb);
});

gulp.task("clean-site", function (cb) {
    return rimraf("wwwroot/lib/site/", cb);
});

gulp.task("clean", gulp.parallel('clean-vendor', 'clean-site'));

gulp.task("scripts", function () {

    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/vendor/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});

gulp.task('default',
    gulp.series('clean', gulp.parallel('minify', 'scripts'))
);