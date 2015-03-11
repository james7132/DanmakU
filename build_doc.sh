git push origin master
doxygen Docs/Doxyfile
cd Docs/html
git checkout gh-pages
git add --all
git commit
git push gh-pages
git checkout master
cd ../..
