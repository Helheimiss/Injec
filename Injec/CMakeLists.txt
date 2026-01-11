cmake_minimum_required(VERSION 4.1)
project(Injec)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)


add_library(${PROJECT_NAME} SHARED src/Injec.cpp)

target_include_directories(${PROJECT_NAME} PRIVATE
        include
)
target_compile_options(${PROJECT_NAME} PRIVATE
        $<$<CONFIG:Release>:-O3>
)
target_link_options(${PROJECT_NAME} PRIVATE
        $<$<CONFIG:Release>:-static>
)