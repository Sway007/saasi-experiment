#!/bin/sh
 docker ps --format "{{.ID}}: {{.Names}} {{.Image}}" &> name.txt