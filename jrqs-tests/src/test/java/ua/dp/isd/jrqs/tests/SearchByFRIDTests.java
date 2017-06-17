package ua.dp.isd.jrqs.tests;

import static org.junit.Assert.assertArrayEquals;
import org.junit.Test;

public class SearchByFRIDTests {

    private WebHelper wh = new WebHelper();
    private final SearchPageHelper searchPage = new SearchPageHelper();
    
    @Test
    public void TestSeacrhByFRID() {
        String searchText = "FR1";
        String[] expected = { searchText };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByNFRID() {
        String searchText = "NFR1";
        String[] expected = { searchText };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhIsCaseInsensitive() {
        String searchText = "fr1".toLowerCase();
        String[] expected = { searchText.toUpperCase() };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralFRIDs() {
        String searchText = "FR1,FR2";
        String[] expected = searchText.split(",");
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralFRIDsWithExtraSpaces() {
        String searchText = " fr1 , fr2,";
        String[] expected = { "FR1", "FR2" };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 2);
        assertArrayEquals(expected, actual);
    }
}
