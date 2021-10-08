#!/bin/bash
if [[ $1 == "Master" ]]; then
  git checkout -b main
  git merge Devel
  git branch -d Devel
  git add .
  git commit -m "version $2 auto commit"
  git push
  git checkout -b "Devel"
else
  git checkout -b Devel
fi
git push
