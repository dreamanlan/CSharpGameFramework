#!/bin/sh

work_dir=$1
source_dll=$2
output_dll=$3
target_dll=$4

cd ${work_dir}
echo "work_dir:${work_dir}"

echo `uname -s` | grep "MINGW"
if [ $? -ne 0 ];then
  echo "Execute on mac/linux/unix"
  export MONO_PATH=/Library/Frameworks/Mono.framework/Libraries/mono/3.5/
  mono ../../Precompile/precompile.exe ${source_dll} -o:${output_dll} -t:${target_dll}
else
  echo "Execute on win"
  ../../Precompile/precompile.exe ${source_dll} -o:${output_dll} -t:${target_dll}
fi
exit 0
