package ua.dp.isd.jrqs.tests;

import java.util.ArrayList;
import java.util.List;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class SearchPageHelper {
    
    private WebDriver d;
    private WebHelper h;
    
    public SearchPageHelper(WebDriver driver) {
        d = driver;
        h = new WebHelper(driver);
    }
    
    public String[] doSearchBy(String text, int column) {
        h.findElement("#value").clear();
        h.findElement("#value").sendKeys(text);
        h.findElement("button[type='submit']").click();
        
        List<String> results = new ArrayList<>();
        for (WebElement e : h.findElements(String.format("table tr td:nth-child(%s)", column))) {
            results.add(e.getText());
        }
        return results.toArray(new String[results.size()]);
    }
}
