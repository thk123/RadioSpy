import os
import re
for filename in os.listdir('TwineSrc'):
    with open('TwineSrc/' + filename, 'r') as f:
    	with open ('Assets/Twine/' + filename, 'w') as output:
    		for line in f:
    			fixed_line = line.replace('hidden><style role=', 'hidden=""><style role=')
    			output.write(fixed_line)
