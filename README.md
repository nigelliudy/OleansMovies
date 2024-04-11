See detailed walkthrough in the second half, [or click here to view on github](https://github.com/nigelliudy/OleansMovies/wiki#detailed-walkthrough-of-movie-creation-duplicate-update-and-searching)

# Quick start

As a quick start guide, this application can be tested at "http://localhost:6600/ui/playground"
![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/fbca2fa9-ba01-411e-b5c7-44a393ce893b)

Ensure that movies.json is copied to the bin directory when running the highlighted application project (silo local). After that, browse to the playground UI.

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/8172930d-b247-47a1-8c3d-5961d3f7fdff)
![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/1f8829e0-f648-451c-a9b9-3b4b8f18c7e1)


# GraphQL playground queries and variables

## Use the following as example queries in playground

```
query Home {
    home_top_5 {
      name,
      rate,
      image
    },
    movies_list(type: "search", search_name: "Dark Knight") {
      id,
      key,
    	name,
      description,
      genres,
      image
    }
}

query Movies_List {
    home(top: 2) {
      name,
      image,
      rate
    },
    movies_list(type: "list") {
      name
    }
}

query Movies_Search($searchName: String!) {
    movies_list(type: "search", search_name: $searchName) {
      name,
      description,
    },
}

query Movies_Genre($searchGenre: String!) {
    movies_list(type: "filter_by_genres", search_genres: $searchGenre) {
      name,
      description,
    	genres,
      rate,
      image
    }
}

mutation Create_Movie($newMovie: MovieInput!) {
    create(movie: $newMovie) {
      id,
      name,
      genres
    }
}

mutation Update_Movie($updateMovie: MovieInput!) {
    update(movie: $updateMovie) {
      id,
      name,
      genres
    }
}

query Movie_Detail($detailMovieId: Int!) {
    movie_detail(id: $detailMovieId) {
      id,
      key,
    	name,
      description,
      genres,
      image
    }
}
```
## Then use these as example variables in playground
```
{
  "searchName": "the",
  "searchGenre": "drama",
  "detailMovieId": 17,
  "newMovie": {
    "id": 0,
    "key": "duplicating-newmovie",
    "name": "New Duplication Movie",
    "description": "A movie that keeps on duplicating endlessly.",
    "genres": [
        "horror",
        "comedy",
        "drama"
    ],
    "rate": "1.0",
    "length": "1hr 1mins",
    "image": "duplicate-new.jpg"
  },
  "updateMovie": {
    "id": 17,
    "key": "running-away",
    "name": "Running Away",
    "description": "Changed from running scared to running away.",
    "genres": [
        "action",
        "crime",
        "drama"
    ],
    "rate": "7.4",
    "length": "2hr 2mins",
    "image": "running-away.jpg"
  }
}
```
# Detailed walkthrough of movie creation, duplicate, update and searching

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/5d32f0f2-3383-49c0-a4b8-2630636c1851)

The objective is to add a movie titled "New Duplication Movie", attempt to add the same movie again, update a another movie, and list all movies.
1. Click on the play button run the query ```Create_Movie```. This will add "New Duplication Movie" from the variable "newMovie".
2. Do the same again.
3. Now choose ```Movies_List``` and you should see the following in the results.

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/c9b24876-85b7-492a-8205-81324de790c5)

Notice how the "New Duplication Movie" can be added twice without the same id. To remove "home" and see the id in "movies_list":
1. Replace ```query Movies_List``` with the following
```
query Movies_List {
    movies_list(type: "list") {
      id,
      name
    }
}
```
2. Confirm that the id for both instances of "New Duplication Movie" are different.

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/342951f0-21d1-4f50-a93e-5264c9522599)

***

Next objective is to update an existing movie at id: 17
1. Confirm that the variables has "updateMovie" like so

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/228c6efd-d8d9-408a-b6ab-9f426570c580)

2. Run the query ```Update_Movie```
3. Confirm the name of movie has changed for id: 17

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/b7d4c5a9-22aa-44e0-9218-c73ca6b75b7b)

***

Next objective is to search by name (or genre) that contain the search term
1. Confirm that the variables "searchName" and "searchGenre" are present

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/9c15e267-ec69-4d63-ad1f-b1330db5c1da)

2. Run the ```Movies_Search``` to match movie names containing "searchName" substring
3. Run the ```Movies_Genre``` to match movie genres containing "searchGenre" substring
4. Note that "movies_list" is being reused, hence result's property display "movies_list" for the above searches.

![image](https://github.com/nigelliudy/OleansMovies/assets/166240092/d0688ece-d01c-465b-93de-b6b2a4bca2f1)

Screenshot example of running ```Movies_Search```
