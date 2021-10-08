git checkout -b $1
if [[ $1 == "Master" ]]; then
  git merge Devel
  git branch -d Devel
  git add .
  git commit -m "version $2 auto commit"
  git push
  git checkout -b "Devel"
fi
