/// <binding Clean='default' />
var gulp = require('gulp'),
    gp_clean = require('gulp-clean'),
    gulp_concat = require('gulp-concat'),
    gulp_sourcemaps = require('gulp-sourcemaps'),
    gulp_uglify = require('gulp-uglify'),
    gulp_typescript = require('gulp-typescript');

var srcPaths = {
    app: ['Scripts/app/main.ts', 'Scripts/app/**/*.ts'],
    js: [
        'Scripts/js/**/*.js',
        'node_modules/core-js/client/shim.min.js',
        'node_modules/zone.js/dist/zone.js',
        'node_modules/reflect-metadata/Reflect.js',
        'node_modules/systemjs/dist/system.src.js',
        'node_modules/typescript/lib/typescript.js'
        ],
    js_angular: [
        'node_modules/@angular/**' 
    ],
    js_rxjs : [
        'node_modules/rxjs/**' 
    ]
}

var destPaths = {
    app: 'wwwroot/app/',
    js: 'wwwroot/js/',
    js_angular: 'wwwroot/js/@angular/',
    js_rxjs: 'wwwroot/js/rxjs/',
}

gulp.task('app', ['app_clean'], function () {
    return gulp.src(srcPaths.app)
        .pipe(gulp_sourcemaps.init())
        .pipe(gulp_typescript(require('./tsconfig.json').compilerOptions))
        //.pipe(gulp_uglify({ mangle: false }))
        .pipe(gulp_sourcemaps.write('/'))
        .pipe(gulp.dest(destPaths.app));
});

gulp.task('app_clean', function () {
    return gulp.src(destPaths.app + "*.*", { read: false })
        .pipe(gp_clean({ force: true }));
});

gulp.task('js', function () {
    gulp.src(srcPaths.js_angular)        
        .pipe(gulp.dest(destPaths.js_angular));
    
    gulp.src(srcPaths.js_rxjs)        
        .pipe(gulp.dest(destPaths.js_rxjs));
    
    return gulp.src(srcPaths.js)
        //.pipe(gulp_uglify({mangle : false})) disabled on the dev environment for debugging
        //.pipe(gulp_concat('alljs.min.js'))
        .pipe(gulp.dest(destPaths.js));
});

gulp.task('js_clean', function () {
    return gulp.src(destPaths.js + "*", { read : false})        
        .pipe(gp_clean({ force: true }));
});

gulp.task('watch', function () {
    return gulp.src(srcPaths.app, srcPaths.js, ['app', 'js']);
});

gulp.task('cleanup', ['app_clean', 'js_clean']);
gulp.task('default', ['app', 'js', 'watch']);


