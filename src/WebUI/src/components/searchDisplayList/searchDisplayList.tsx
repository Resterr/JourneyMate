import * as React from 'react';
import './searchDisplayList.css';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Checkbox from '@mui/material/Checkbox';
import IconButton from '@mui/material/IconButton';
import SearchDisplayListModal from "./modal/searchDisplayListModal";
import {Place} from "../../models/Place";

type PlacesListProps = {
	places : Place[];
	onSelectedPlacesChange : (checked : string[]) => void;
}

const SearchDisplayList : React.FC<PlacesListProps> = ({places, onSelectedPlacesChange}) => {
	const [checked, setChecked] = React.useState<string[]>([]);

	const handleToggle = (value : string) => () => {
		const currentIndex = checked.indexOf(value);
		const newChecked = [...checked];

		if (currentIndex === -1) {
			newChecked.push(value);
			
		} else {
			newChecked.splice(currentIndex, 1);
		}
		setChecked(newChecked);
		onSelectedPlacesChange(newChecked);
	};
    
	return (
		<div className="searchDisplay__list">
			<List sx={{width: '100%', maxWidth: 1400}}>
				{places.map((place) => {
					const labelId = `checkbox-list-label-${place.id}`;
	
					return (
						<ListItem
							key={place.id}
							secondaryAction={
								<IconButton edge="end" aria-label="comments" >
									<SearchDisplayListModal placeDetails={place}></SearchDisplayListModal>
								</IconButton>
							}
							disablePadding
						>
							<ListItemButton role={undefined} onClick={handleToggle(place.id)} dense>
								<ListItemIcon>
									<Checkbox
										edge="start"
										checked={checked.indexOf(place.id) !== -1}
										tabIndex={-1}
										disableRipple
										inputProps={{'aria-labelledby': labelId}}
									/>
								</ListItemIcon>
								<ListItemText id={labelId} primary={`${place.name} - ${place.vicinity}`}/>
							</ListItemButton>
						</ListItem>
					);
				})}
			</List>
		</div>
	);
}

export default SearchDisplayList;