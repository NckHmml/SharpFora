/// <binding BeforeBuild="default" Clean="clean" ProjectOpened="watch" />
"use strict";

// Wraped the gulp file to prevent references spilling
(function () { 
    var gulp = require("gulp"),
        $ = require("gulp-load-plugins")(),
        paths = {
            contents: "./wwwroot/contents/",
            wrap: "Scripts/wrap.template.js",
            sourcemaps: "/source_maps",
            javascript: {
                src: "Scripts/Public/**/*.js",
                output: "main.min.js"
            },
            sass: {
                main: "Style/main.scss",
                src: "Style/**/*"
            },
            templates: {
                src: "Templates/**/*.html",
                output: "Templates.cshtml",
                path: "Views/Home/"
            }
        };

    gulp.task("sass", function () {
        gulp.src(paths.sass.main)
            .pipe($.sourcemaps.init())
            .pipe($.sass({ outputStyle: "compressed" }).on("error", $.sass.logError))
            .pipe($.cssmin())
            .pipe($.sourcemaps.write(paths.sourcemaps))
            .pipe(gulp.dest(paths.contents));
    });

    gulp.task("javascript", function () {
        gulp.src(paths.javascript.src)
            .pipe($.sourcemaps.init())
            .pipe($.concat(paths.javascript.output))
            .pipe($.wrap({ src: paths.wrap }))
            .pipe($.uglify())
            .pipe($.sourcemaps.write(paths.sourcemaps))
            .pipe(gulp.dest(paths.contents));
    });

    gulp.task("templates", function () {
        gulp.src(paths.templates.src)
            .pipe($.concat(paths.templates.output))
            .pipe($.htmlmin({
                collapseWhitespace: true,
                processScripts: ["text/ng-template"]
            }))
            .pipe($.insert.prepend("@{\n\t//This file is auto generated\n\tLayout=null;\n}\n"))
            .pipe(gulp.dest(paths.templates.path));
    });

    gulp.task("clean:css", function () {
        gulp.src(paths.contents + "/**/*.css", { read: false })
            .pipe($.rimraf());
    });

    gulp.task("clean:maps", function () {
        gulp.src(paths.contents + "/**/*.map", { read: false })
            .pipe($.rimraf());
    });

    gulp.task("clean:javascript", function () {
        gulp.src(paths.contents + "/**/*.js", { read: false })
            .pipe($.rimraf());
    });

    gulp.task("default", ["sass", "javascript", "templates"]);

    gulp.task("clean", ["clean:css", "clean:javascript", "clean:maps"]);

    gulp.task("watch", function () {
        gulp.watch(paths.sass.src, ["sass"]);
        gulp.watch(paths.javascript.src, ["javascript"]);
        gulp.watch(paths.templates.src, ["templates"]);
    });
})();