const gulp = require('gulp');
const debug = require('gulp-debug');
const babel = require('gulp-babel');
const terser = require('gulp-terser');

gulp.task('processCSS', gulp.series((done) => {
  gulp.src('Scripts/css/*.css')
    .pipe(debug())
    .pipe(gulp.dest('wwwroot/css'));
  console.log("done running processCSS");
  done();
}));


gulp.task('processJS', gulp.series((done) => {

  gulp.src([
    'Scripts/js/*.js'
  ])
    // .pipe(debug())
    // .pipe(babel({
    //   presets: ['@babel/preset-env']
    // }))
    // .pipe(terser()) 
    .pipe(gulp.dest('wwwroot/js'))
  console.log("done running processJS");
  done();
}));

gulp.task('default', gulp.series(
  'processJS', 'processCSS',
));