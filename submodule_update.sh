git submodule update --recursive
git submodule foreach git add --all
git submodule foreach git commit
git submodule foreach git push
git add --all
git commit
git push