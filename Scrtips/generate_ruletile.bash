# indicies=(
#     $1+0
#     $1+1
#     $1+2
#     $1+41
#     $1+42
#     $1+80
#     $1+81
#     $1+82
#     $1+120
#     $1+122
#     $1+160
#     $1+161
#     $1+162
# )

indicies=( $(($1+0)) $(($1+1)) $(($1+2)) $(($1+41)) $(($1+42)) $(($1+80)) $(($1+81)) $(($1+82)) $(($1+120)) $(($1+122)) $(($1+160)) $(($1+161)) $(($1+162)))

res=`cat ruletile$2_template.txt`
res=${res//#00/$1}
# $1: MainTile idx
# $2: Tiles type

cnt=1
for idx in ${indicies[@]};
    do 
        lineIdx=`awk '/tiles'$2'_'$idx'/{print NR-1}' tiles$2.png.meta | head -n1`; 
        newId=`echo $(sed ''$lineIdx'q;d' tiles$2.png.meta) | cut -d ' ' -f2-`
        i=`printf "%02d" $cnt`
        res="${res//#${i}/${newId}}"
        ((cnt++))
done

echo "$res" > "../Assets/Tiles/RuleTiles$2/$1.asset"