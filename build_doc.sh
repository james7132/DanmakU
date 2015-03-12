git push origin master
doxygen Docs/Doxyfile
mkdir ../~temp
mv Docs/html ../~temp
git checkout gh-pages
rm -r --force Docs/html
mv ../~temp/html Docs
