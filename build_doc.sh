git push origin master >> ../doc_log.sh
doxygen Docs/Doxyfile >> ../doc_log.sh
mkdir ../~temp >> ../doc_log.sh
mv Docs/html ../~temp >> ../doc_log.sh
git checkout gh-pages >> ../doc_log.sh
rm -r --force Docs/html >> ../doc_log.sh
mv ../~temp/html Docs >> ../doc_log.sh
rm ../~temp >> ../doc_log.sh
git add --all >> ../doc_log.sh
git commit --message="Documentation Update" >> ../doc_log.sh
git push >> ../doc_log.sh
git checkout master >> ../doc_log.sh
