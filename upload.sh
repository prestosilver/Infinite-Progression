#!/bin/bash
if [[ $1 == "Master" ]]; then
  git checkout
  git fetch
  git merge Devel
  git branch -d Devel
  git add .
  git commit -m "version $2 auto commit"
  git push
  git checkout -b "Devel"
else
  git checkout Devel
fi
git push
